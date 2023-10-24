using System.Reflection;
using SharedKernel.Core;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;

namespace SharedKernel.Log;

public static class Logging
    {
        public static object _lockObj = new object();

        public static void Information(string information, bool isDebug = false)
        {
            try
            {
                if (!isDebug)
                {
                    var fileName = $"information-{DateHelper.Now.ToString("dd-MM-yyyy")}.txt";
                    var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/log";

                    Directory.CreateDirectory(path);
                    lock (_lockObj)
                    {
                        using (var w = File.AppendText($"{path}/{fileName}"))
                        {
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine($"{DateHelper.Now.DateFullText()}");
                            w.WriteLine(information);
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CatchableException(ex.Message);
            }
            finally
            {
                DefaultLoggingConfig.Logger.Information(information);
            }
        }

        public static void Warning(string information, bool isDebug = false)
        {
            try
            {
                if (!isDebug)
                {
                    var fileName = $"warning-{DateHelper.Now.ToString("dd-MM-yyyy")}.txt";
                    var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/log";

                    Directory.CreateDirectory(path);
                    lock (_lockObj)
                    {
                        using (var w = File.AppendText($"{path}/{fileName}"))
                        {
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine($"{DateHelper.Now.DateFullText()}");
                            w.WriteLine(information);
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CatchableException(ex.Message);
            }
            finally
            {
                DefaultLoggingConfig.Logger.Warning(information);
            }
        }

        public static void Error(Exception exception, string moreInfo = "", bool isDebug = false)
        {
            try
            {
                if (!isDebug)
                {
                    var fileName = $"error-{DateHelper.Now.ToString("dd-MM-yyyy")}.txt";
                    var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/log";

                    Directory.CreateDirectory(path);
                    lock (_lockObj)
                    {
                        using (var w = File.AppendText($"{path}/{fileName}"))
                        {
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine(DateHelper.Now.DateFullText());
                            w.WriteLine("------- StackTrace:");
                            w.WriteLine(exception.StackTrace);
                            w.WriteLine("------- Message:");
                            w.WriteLine(exception.Message);
                            if (!string.IsNullOrWhiteSpace(moreInfo))
                            {
                                w.WriteLine($"------- More info: {moreInfo}");
                            }
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CatchableException(ex.Message);
            }
            finally
            {
                DefaultLoggingConfig.Logger.Error(exception, "");
            }
        }

        public static void Error(string information, bool isDebug = false)
        {
            try
            {
                if (!isDebug)
                {
                    var fileName = $"error-{DateHelper.Now.ToString("dd-MM-yyyy")}.txt";
                    var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/log";

                    Directory.CreateDirectory(path);
                    lock (_lockObj)
                    {
                        using (var w = File.AppendText($"{path}/{fileName}"))
                        {
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine(DateHelper.Now.DateFullText());
                            w.WriteLine($"Information: {information}");
                            w.WriteLine("__________________________________________________________________________________________________________________________________________________________________________________________");
                            w.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CatchableException(ex.Message);
            }
            finally
            {
                DefaultLoggingConfig.Logger.Error(information);
            }
        }

        public static void LogCustom(string file, string information, bool isDebug = false)
        {
            try
            {
                if (!isDebug)
                {
                    var fileName = $"{file}-{DateHelper.Now.ToString("dd-MM-yyyy")}.txt";
                    var path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/log";

                    Directory.CreateDirectory(path);
                    lock (_lockObj)
                    {
                        using (var w = File.AppendText($"{path}/{fileName}"))
                        {
                            w.WriteLine($"{DateHelper.Now.DateFullText()}: {information}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CatchableException(ex.Message);
            }
            finally
            {
                DefaultLoggingConfig.Logger.Information(information);
            }
        }

    }