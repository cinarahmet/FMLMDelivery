
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FMLMDelivery
{
    class Csv_Writer
    {
        private readonly StreamWriter _w;
                
        private List<String> _records;

        private String _header;

        private String _filename;
        public Csv_Writer(List<String> records_of_outcomes, String filename, String Header)
        {
            _records = records_of_outcomes;
            _header = Header;
            _filename = filename;
            var filepath = @"C:\NETWORK DESIGN\FMLMDelivery\FMLMDelivery\bin\Debug\" + filename + ".csv";
            _w = new StreamWriter(filepath,false,Encoding.UTF8);
            _w.WriteLine(Header);
        }

        public void Write_Records()
        {
            for (int i = 0; i < _records.Count; i++)
            {   
                _w.WriteLine(_records[i]);
                
                _w.Flush();
            }
        }
    }
}
