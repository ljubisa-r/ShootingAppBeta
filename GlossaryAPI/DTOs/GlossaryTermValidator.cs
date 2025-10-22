namespace GlossaryAPI.DTOs
{
    public class GlossaryTermValidator
    {
        
        public void ValidateTermForPublish(GlossaryTermDTO term)
        {
            if (term == null)
                throw new ArgumentNullException(nameof(term));
            if (string.IsNullOrWhiteSpace(term.term))
                throw new ArgumentException("Term name is required.");
            if (string.IsNullOrWhiteSpace(term.definition) || term.definition.Length <= 30)
                throw new UnauthorizedAccessException("Definition must be longer than 30 characters.");
            
            var forbiddenWords = new[] { "lorem", "test", "sample" };
           
            var found = forbiddenWords
                .Where(word => term.definition.Contains(word, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (found.Any())
            {
                throw new UnauthorizedAccessException(
                    $"Definition contains forbidden words: {string.Join(", ", found)}.");
            }
        }
    }

}
