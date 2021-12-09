using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.Linq;

namespace Coursework_AaDS
{
    static class XmlParsingUtility
    {
        public static IEnumerable<OperationWorker> GetOperations()
        {
            XmlDocument document = new XmlDocument();
            document.Load("OperationsAndConsts.xml");
            XmlElement xRoot = document.DocumentElement;

            List<Type> workers = Assembly.GetExecutingAssembly().GetTypes()
                .Where((Type type) =>
                    type.IsSubclassOf(typeof(OperationWorker)) &&
                    !type.IsAbstract)
                .ToList();


            foreach (XmlNode node in xRoot)
            {
                if (node.Name == "OperationClass")
                {
                    OperationType type=OperationType.None;
                    string syntax = null;
                    int priority = -1;
                    Type workerClass = null;
                    foreach (XmlNode childNode in node.ChildNodes)
                    {


                        // --------SYNTAX---------

                        if (childNode.Name == "syntax")
                        {
                            syntax = childNode.InnerText;
                        }

                        // --------PRIORITY-------

                        else if (childNode.Name == "priority")
                        {
                            priority = Convert.ToInt32(childNode.InnerText);

                        }

                        // ---------ARG PLACING----------

                        else if (childNode.Name == "type")
                        {

                            switch (childNode.InnerText)
                            {
                                case "UnaryPrefix":
                                case "Unary":
                                    type = OperationType.UnaryPrefix;
                                    break;
                                case "UnaryPostfix":
                                    type = OperationType.UnaryPostfix;
                                    break;
                                case "Binary":
                                    type = OperationType.Binary;
                                    break;
                                default:
                                    type = OperationType.None;
                                    break;
                            }
                        }

                        // ---------WORKER---------

                        else if (childNode.Name == "worker")
                        {
                            string className = childNode.Attributes["class"].InnerText;
                            foreach (var attr in workers)
                            {
                                if (attr.Name == className)
                                {
                                    workerClass = attr;
                                    break;
                                }
                            }
                        }
                    }

                    yield return (OperationWorker)workerClass
                        .GetConstructor(new Type[] 
                            { typeof(string), typeof(int), typeof(OperationType)})
                        .Invoke(new object[]{ syntax, priority, type });


                }

            }
        }
    }
}
