﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Shared.Util
{
    public class Reader
    {
        public static Exam FromOefFile (string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Empty filepath");
            else if (!File.Exists(filePath))
                throw new FileNotFoundException("File specified does not exist");
            else
            {
                Stream stream = null;
                try
                {
                    
                    IFormatter formatter = new BinaryFormatter();
                    stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    Exam exam = (Exam)formatter.Deserialize(stream);
                    return exam;
                }
                catch(Exception)
                {
                    return null;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }            
        }

        public static Exam FromJsonFile(string filePath)
        {
            Exam exam = null;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string jsonString = streamReader.ReadToEnd();
                exam = JsonConvert.DeserializeObject<Exam>(jsonString);
            }
            return exam;
        }

        public static Exam FromXmlFile(string filePath)
        {
            Exam exam = null;
            var xmlSerializer = new XmlSerializer(typeof(Exam));
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                exam = (Exam)xmlSerializer.Deserialize(streamReader);
            }
            return exam;
        }

        public static bool WriteExamToOefFile (Exam exam, string filePath)
        {
            if (exam == null)
                throw new NullReferenceException("The exam to be written cannot be null.");
            else if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Empty filepath");
            Stream stream = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, exam);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }
}
