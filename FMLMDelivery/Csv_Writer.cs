
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FMLMDelivery.Classes
{
    class Csv_Writer
    {
        private readonly StreamWriter _w;
                
        private List<String> _records;

        private String _header;

        private String _filename;

        private String _output_directory;
        public Csv_Writer(List<String> records_of_outcomes, String filename, String Header,String output_directory)
        {
            _records = records_of_outcomes;
            _header = Header;
            _filename = filename;
            _output_directory = output_directory;
            var filepath = @""+ _output_directory+"\\" + filename + ".csv";
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
