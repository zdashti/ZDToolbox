using System.ComponentModel.DataAnnotations;

namespace ZDToolbox
{
    public enum ContentTypeEnum
    {
        [Display(Name = "application/json")] ApplicationJson,

        [Display(Name = "application/x-www-form-urlencoded")]
        ApplicationXWwwFormUrlencoded
    }
}