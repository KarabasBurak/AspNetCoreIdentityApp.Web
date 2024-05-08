using AspNetCoreIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.Extenisons;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleListDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View(roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateDto roleCreateDto)
        {
            var newRole = new AppRole() 
            { 
                Name = roleCreateDto.Name 
            };
            var result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            TempData["SuccessMessage"] = "Rol Oluşturuldu";

            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleUpdate=await _roleManager.FindByIdAsync(id);
            if (roleUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamıştır");
            }
            return View(new RoleUpdateDto() { Id=roleUpdate.Id, Name=roleUpdate.Name});
        }

        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateDto roleUpdateDto)
        {
            var roleUpdate=await _roleManager.FindByIdAsync(roleUpdateDto.Id);
            if(roleUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamıştır");
            }

            roleUpdate.Name = roleUpdateDto.Name;
            await _roleManager.UpdateAsync(roleUpdate);

            ViewData["SuccessMessage"] = "Rol bilgisi güncellenmiştir";
            return View();
        }

        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleDelete=await _roleManager.FindByIdAsync(id);
            if (roleDelete == null)
            {
                throw new Exception("Silinecek Rol Bulunamadı");
            }


            var result = await _roleManager.DeleteAsync(roleDelete);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());
            }

            TempData["SuccessMessage"] = "Rol Silindi";

            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id); // Kullanıcıları çektik
            var currentRoles=await _roleManager.Roles.ToListAsync(); // Mevcut rolleri çektik
            var userRoles = await _userManager.GetRolesAsync(currentUser); // UserManager sınıfındaki GetRolesAsync metodu ile kullanıcılara rol atanmış rolleri çekiyoruz
            var assignRoleToUserDtoList= new List<AssignRoleToUserDto>(); // AssignRoleToUserDto sınıfındaki nesnelerden nesne örneği oluşturduk
            

            foreach(var role in currentRoles)  // Mevcut rolleri döngüye aldık
            {
                var assignRole = new AssignRoleToUserDto() //AssignRoleToUserDto sınıfındaki Id ve Name proplara atama yaptık
                {
                    Id= role.Id,
                    Name= role.Name,
                };

                if (userRoles.Contains(role.Name)) // Kullanıcının rolüne bir rol ataması yapılmış mı kontrol ediyoruz.
                {
                    assignRole.Exist = true;  
                }

                assignRoleToUserDtoList.Add(assignRole);  // Eğer rol atanmamışsa kullanıcıya rol ekliyoruz.

            }

            return View(assignRoleToUserDtoList);
        }

        public async Task<IActionResult> AssignRoleToUser(List<AssignRoleToUserDto> assignRoleToUserDto)
        {
            return View();
        }

    }
}
