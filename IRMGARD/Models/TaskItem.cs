using Newtonsoft.Json;

namespace IRMGARD.Models
{
    public class TaskItem
    {
        public TaskLetter TaskLetter { get; set; } 
        public Media Media { get; set; }
        public bool IsSearched { get; set; }
        public bool IsDirty { get; set; }

        [JsonConstructor]
        public TaskItem(TaskLetter taskLetter, Media media, bool isSearched, bool emptySearchedItems = false)
        {
            TaskLetter = taskLetter;
            Media = media;
            IsSearched = isSearched;

            // Make letter empty it when is searched
            if (emptySearchedItems && isSearched)
            {
                if (TaskLetter != null)
                    TaskLetter.Letter = "";
                if (Media != null)
                    Media = null;
            }
        }
    }
}