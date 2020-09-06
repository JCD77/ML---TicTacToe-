using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TicTacToe.ModeloAprendizaje;

namespace TicTacToe
{
   public class PlayerIA : ILevel
    {
        MLContext mlContext;
        int _seed = new Random().Next(0, int.MaxValue);
        double _level = 1;
        bool _random = false;
        public PlayerIA(){
            mlContext=   new MLContext(seed:_seed);
            this.Entrenar();
            }
        public PlayerIA(double level,bool random=false):base()
        {
            mlContext= new MLContext(seed: _seed);
            _level = level;
            this._random = random;
            this.Entrenar();
        }
           
        public NivelJugador Getlevel()
        {
            return NivelJugador.LEVELIA;
        }

        public Partida SiguienteMovimiento(Partida partida)
        {

            var posicionesEvaluar = partida.PosicionesSiguientes(partida.Turno);
            List<Ficha[,]> posicionesEvaluarNormalizada = new List<Ficha[,]>();

            List<Ficha[,]> newList = new List<Ficha[,]>(posicionesEvaluar.Count);

            posicionesEvaluar.ForEach((item) =>
            {
                Ficha[,] nuevo = (Ficha[,])item.Clone();
                posicionesEvaluarNormalizada.Add(nuevo);
            });

            //Si el turno es del jugador2 se intercambian las fichas
            if (partida.Turno == Turno.JUGADOR2)
            {
                for (int i1 = 0; i1 < posicionesEvaluarNormalizada.Count; i1++)
                {
                    posicionesEvaluarNormalizada[i1] = partida.FlipTablero(posicionesEvaluarNormalizada[i1]);

                }
            }

            List<InputData> inputData = new List<InputData>();
            foreach (var a in posicionesEvaluarNormalizada)
            {
                InputData input = new InputData();
                input.Features = new float[9];

                input.Features[0] = ((int)a[0, 0]) / (float)2.0;
                input.Features[1] = ((int)a[0, 1]) / (float)2.0;
                input.Features[2] = ((int)a[0, 2]) / (float)2.0;
                input.Features[3] = ((int)a[1, 0]) / (float)2.0;
                input.Features[4] = ((int)a[1, 1]) / (float)2.0;
                input.Features[5] = ((int)a[1, 2]) / (float)2.0;
                input.Features[6] = ((int)a[2, 0]) / (float)2.0;
                input.Features[7] = ((int)a[2, 1]) / (float)2.0;
                input.Features[8] = ((int)a[2, 2]) / (float)2.0;


                inputData.Add(input);


            }
            string fichero = "Level" + (int)(_level * 100) + ".zip";

            var model = mlContext.Model.Load(fichero, out var predictionPipelineSchema);



            var predictionEngine = mlContext.Model.CreatePredictionEngine<InputData, OutputData>(model);
            OutputData prediction;
            InputData maximo;
            float valor = -1;
            int indiceMaximo = 0;
            int prediccionScoreAcumulado = 0;
            int i = 0;
            List<scoreMovimiento> listaResultado = new List<scoreMovimiento>();
            foreach (var movimiento in inputData)
            {

                prediction = predictionEngine.Predict(movimiento);
                if (prediction.Porcentaje > valor)
                {
                    valor = prediction.Porcentaje;
                    maximo = movimiento;
                    indiceMaximo = i;
                }
                scoreMovimiento score = new scoreMovimiento();
                score.posicion = i;
                score.score = Math.Abs((int)(prediction.Porcentaje * 100));
                prediccionScoreAcumulado += Math.Abs(score.score);
                listaResultado.Add(score);
                i++;
            }

            if (_random)
            {
                int contador = 100;
                //Rellenamos 
                listaResultado = listaResultado.OrderByDescending(x => x.score).ToList();
                for (int j = 0; j < listaResultado.Count(); j++)
                {

                    var aux = new scoreMovimiento();
                    aux.posicion = listaResultado[j].posicion;
                    aux.score = listaResultado[j].score;
                    aux.probabilidadMinima = contador - ((int)(aux.score * 100 / ((prediccionScoreAcumulado == 0) ? 1 : prediccionScoreAcumulado)));
                    aux.probabilidadMaxima = contador;
                    if (aux.probabilidadMinima < 0) aux.probabilidadMinima = 0;
                    contador = aux.probabilidadMinima - 1;
                    listaResultado[j] = aux;

                }
                //Ficha[,] posicion = new Ficha[partida.Rango, partida.Rango];
                //
                //partida.Turno = partida.Turno == Turno.JUGADOR1 ? Turno.JUGADOR2 : Turno.JUGADOR1;


                int probabilidad = new Random().Next(0, 100);

                int indice = 0;
                try
                {
                    indice = listaResultado.Where(x => x.probabilidadMinima <= probabilidad && x.probabilidadMaxima >= probabilidad).First().posicion;
                }
                catch
                {

                }

                partida.Tablero = posicionesEvaluar[indice];
            }
            else
            {
                partida.Tablero = posicionesEvaluar[indiceMaximo];
            }
            partida.Turno = partida.Turno == Turno.JUGADOR1 ? Turno.JUGADOR2 : Turno.JUGADOR1;





            return partida;
        }
      

        public  void Entrenar()
        {
            List<InputData> lista = ModeloAprendizaje.Estructuras2List();

            System.Console.WriteLine("Emprezando Entrenamiento: " + this.Descripcion());
            IDataView dataview = mlContext.Data.LoadFromEnumerable<InputData>(lista);


            //10% para testing
            DataOperationsCatalog.TrainTestData dataSplit = mlContext.Data.TrainTestSplit(dataview, testFraction: _level);
            IDataView trainData = dataSplit.TrainSet;
            IDataView testData = dataSplit.TestSet;


            IEstimator<ITransformer> dataPrepEstimator =
            mlContext.Transforms.NormalizeMinMax("Features");


            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(trainData);
            IDataView transformedTrainingData = dataPrepTransformer.Transform(trainData);

            var sdcaEstimator = mlContext.Regression.Trainers.Sdca();

            var trainedModel = sdcaEstimator.Fit(transformedTrainingData);


            //TESTEO
            // Measure trained model performance
            // Apply data prep transformer to test data
            IDataView transformedTestData = dataPrepTransformer.Transform(testData);

            // Use trained model to make inferences on test data
            IDataView testDataPredictions = trainedModel.Transform(transformedTestData);

            // Extract model metrics and get RSquared
            RegressionMetrics trainedModelMetrics = mlContext.Regression.Evaluate(testDataPredictions);
            double rSquared = trainedModelMetrics.RSquared;
            System.Console.WriteLine("ERROR {0}", rSquared);
            string fichero = "Level" + (int)(_level * 100) + ".zip";
            mlContext.Model.Save(trainedModel, dataview.Schema, fichero);
           


            System.Console.WriteLine("Fin Entrenamiento: " + this.Descripcion());


        }

        NivelJugador ILevel.Getlevel()
        {
            return NivelJugador.LEVELIA;
        }

        public double GetIA()
        {
            return _level;
        }

        public string Descripcion()
        {
            return this.Getlevel() + "(" + ((int)(GetIA()*100))+")";
        }
    }
}
