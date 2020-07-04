using Nest;

namespace Search.Models.Elasticsearch
{
    public static class Indices
    {
        public const string IndexAnalyzerName = "autocomplete";
        public const string SearchAnalyzerName = "autocomplete_search";

        /// <summary>
        /// I've moved this into an extension method
        /// for reuse and a clearer understanding of the
        /// custom analyzer we are writing
        /// </summary>
        /// <param name="analysis"></param>
        /// <returns></returns>
        public static IAnalysis AddSearchAnalyzer(this AnalysisDescriptor analysis)
        {
            const string lowercase = nameof(lowercase);
            return
                analysis
                    .Analyzers(a => a
                        .Custom(IndexAnalyzerName, c => c
                            .Tokenizer(IndexAnalyzerName)
                            .Filters(lowercase)
                        )
                        .Custom(SearchAnalyzerName, c =>
                            c.Tokenizer(lowercase)
                        )
                    )
                    .Tokenizers(t => t
                        .EdgeNGram(IndexAnalyzerName, e => e
                            .MinGram(1)
                            .MaxGram(20)
                            .TokenChars(TokenChar.Letter)
                        )
                    );
        }
    }
}