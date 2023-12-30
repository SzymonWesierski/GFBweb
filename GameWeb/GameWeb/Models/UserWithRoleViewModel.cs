using GameWeb.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameWeb.Models;

public class UserWithRoleViewModel
{
    public ApplicationUsers user {  get; set; }
    public String RoleName {  get; set; }
}

