using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.Common.DAL.EF
{

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TId"></typeparam>
  public interface IEntity<TId> where TId : IComparable
  {

    TId Id { get; set; }

    DateTime CreatedDate { get; set; }
         
    DateTime? UpdatedDate { get; set; } 

    bool IsDeleted { get; set; }

  }

  /// <summary>
  /// 
  /// </summary>
  public interface IEntity : IEntity<long>
  {

  }

  public abstract class BaseEntity<TId> : IEntity<TId> where TId : IComparable
  {

    [Required]
    [Key]
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual TId Id { get; set; }

    [ScaffoldColumn(false)]
    [Column(TypeName = "datetime2")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime CreatedDate { get; set; }

    [ScaffoldColumn(false)]
    [Column(TypeName = "datetime2")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime? UpdatedDate { get; set; } 

    [ScaffoldColumn(false)]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

  }

  public abstract class BaseEntity : BaseEntity<long>
  {
  }

}