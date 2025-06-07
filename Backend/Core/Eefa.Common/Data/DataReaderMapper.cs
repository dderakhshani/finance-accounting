
using System;
using System.Collections.Generic;


using System.Data;

namespace Eefa.Common.Data
{
    public class DataReaderMapper<U> where U : class
    {
        
        public List<U> MapProp(DataTable TSource, U TDestination)
        {

            List<U> _EnumsDtos = new List<U>();

            foreach (DataRow dtRow in TSource.Rows)
            {
                U _EnumsDto;
                _EnumsDto = Mapp(TSource, dtRow, TDestination);
                _EnumsDtos.Add(_EnumsDto);
            };

            return _EnumsDtos;
        }

        private U Mapp(DataTable TSource, DataRow row, U TDestination)
        {

            foreach (DataColumn dc in TSource.Columns)
            {
                if (row[dc] != DBNull.Value)
                {
                    object sourceVal = row[dc];
                    string sourceName = dc.ColumnName;
                    TDestination.GetType().GetProperty(sourceName).SetValue(TDestination, sourceVal, null);
                }
                
            }

            return TDestination;
        }


        
    }
   
}


