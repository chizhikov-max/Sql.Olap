using System;
using System.Collections.Generic;
using Sql.Olap.Common;
using Sql.Olap.DataExtractors;
using Sql.Olap.OlapBuilders;

namespace Sql.Olap
{
    public class ExecutorRecalc<In, Out>
        where In : InParametersBase, new()
        where Out : OutParametersBase
    {
        #region [ Ctor ]

        public ExecutorRecalc(ConfigRecalc<In, Out> config)
        {
            this.config = config;

            processor = new Processor<In, Out>(this.config);
            dataExtractor = new BulkDataExtractor<Out>(this.config.GetOlapConfig(processor.GetData<Out>));
            builder = new Builder<Out>(this.config.GetOlapConfig(processor.GetData<Out>));
        }

        #endregion

        #region [ Fields ]

        private readonly ConfigRecalc<In, Out> config;
        private readonly IDataExtractor dataExtractor;
        private readonly IBuilder builder;
        private readonly IProcessor processor;

        #endregion

        public void Run()
        {
            processor.Execute();
            dataExtractor.ClearObsoleteTables();
            builder.ClearObsoleteOlapDatabase();

            dataExtractor.CreateTable();
            if (config.IsTruncateData) dataExtractor.TruncateData();
            dataExtractor.ReadData();
            dataExtractor.InsertData();

            if (config.IsCreateCube)
            {
                builder.GenerateXMLA();
            }
            builder.Process();
        }

        public IEnumerable<string> RunWithNotification()
        {
            #region [ Начало построения ]

            DateTime startTime = DateTime.Now, currentTime = DateTime.Now, lastCurrentTime;
            yield return $"[{currentTime:G}] Начало построения";

            #endregion

            #region [ Формирование данных в Fb ]

            processor.Execute();
            lastCurrentTime = currentTime;
            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Формирование данных в Fb ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";

            #endregion

            #region [ Удаление устаревших таблиц ]

            dataExtractor.ClearObsoleteTables();
            lastCurrentTime = currentTime;
            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Удаление устаревших таблиц ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";

            #endregion

            #region [ Удаление устаревших Olap-баз ]

            builder.ClearObsoleteOlapDatabase();
            lastCurrentTime = currentTime;
            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Удаление устаревших Olap-баз ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";

            #endregion

            #region [ Создание таблицы ]

            bool isCreated = dataExtractor.CreateTable();
            if (isCreated)
            {
                lastCurrentTime = currentTime;
                currentTime = DateTime.Now;
                yield return $"[{currentTime:G}] Создание таблицы для хранения данных ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";
            }

            #endregion

            #region [ Очистка данных ]

            if (config.IsTruncateData)
            {
                dataExtractor.TruncateData();
                lastCurrentTime = currentTime;
                currentTime = DateTime.Now;
                yield return $"[{currentTime:G}] Очистка данных ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";
            }

            #endregion

            #region [ Получение данных ]

            dataExtractor.ReadData();
            lastCurrentTime = currentTime;
            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Получение данных ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";

            #endregion

            #region [ Сохранение данных ]

            //var xml = list.DataContractSerializeObject();
            //File.WriteAllText("c:\\temp\\bcp\\11.xml", xml);
            dataExtractor.InsertData();
            lastCurrentTime = currentTime;
            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Сохранение данных ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";

            #endregion

            #region [ Генерация структуры куба ]

            if (config.IsCreateCube)
            {
                builder.GenerateXMLA();
                lastCurrentTime = currentTime;
                currentTime = DateTime.Now;
                yield return $"[{currentTime:G}] Генерация структуры куба ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";
            }

            #endregion

            #region [ Построение куба ]

            builder.Process();
            lastCurrentTime = currentTime;
            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Построение куба ({currentTime - lastCurrentTime:hh\\:mm\\:ss\\.ff})";

            #endregion

            #region [ Завершение ]

            currentTime = DateTime.Now;
            yield return $"[{currentTime:G}] Построение завершено. Общее время построения: {currentTime - startTime:hh\\:mm\\:ss\\.ff}";

            #endregion
        }
    }
}