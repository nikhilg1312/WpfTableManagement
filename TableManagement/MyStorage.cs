using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TableManagement
{
    class MyStorage
    {

        internal static void SaveXML<T>(T data, string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream stream;
                stream = new FileStream(filename, FileMode.Create);
                serializer.Serialize(stream, data);
                stream.Close();
            }
            catch (Exception)
            {

            }
        }

        internal static T ReadXML<T>(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(sr);
                }
            }
            catch (Exception)
            {

                return default(T);
            }
        }
    }
}
