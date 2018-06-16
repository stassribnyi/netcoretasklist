namespace TaskList.DAL.Models
{
    /// <summary>
    /// Class describing task model
    /// </summary>
    /// <seealso cref="TaskList.DAL.Models.BaseModel" />
    public class TaskModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
