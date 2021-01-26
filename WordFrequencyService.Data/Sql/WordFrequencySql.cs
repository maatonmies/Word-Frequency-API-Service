using System;
using System.Collections.Generic;
using System.Text;

namespace WordFrequencyService.Data.Sql
{
    public static class WordFrequencySql
    {
        public static string GetAllRecordsForTopHundredWords { get; set; } = @"
        ;WITH topwords AS (
        SELECT TOP 100 Word, MAX(TotalFrequency) AS MaxFrequency
        FROM WordFrequency
        GROUP BY Word
        ORDER BY MAX(TotalFrequency) DESC)
        SELECT wf.* 
        FROM WordFrequency wf
        JOIN topwords tw ON tw.Word = wf.Word";
    }
}
