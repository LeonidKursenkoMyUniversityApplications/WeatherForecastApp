//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.EF.WeatherForecast
{
    using System;
    using System.Collections.Generic;
    
    [Table("ImagePath")]
    public partial class ImagePath
    {
        public ImagePath()
        {
            this.DayWeather = new HashSet<DayWeather>();
        }
        
        [Key]
        public int Id { get; set; }
        public string Weather { get; set; }
        public string Path { get; set; }
    
        public virtual ICollection<DayWeather> DayWeather { get; set; }
    }
}
