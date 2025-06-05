using Microsoft.AspNetCore.Mvc.Rendering;

namespace TakeHomeChallenge.Models
{
    public class DropDownList
    {
        public string SelectedItem { get; set; } = "";
        public List<SelectListItem> Items { get; set; } = [];
    }
}
