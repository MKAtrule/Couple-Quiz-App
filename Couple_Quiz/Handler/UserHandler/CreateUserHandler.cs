using AutoMapper;
using Couple_Quiz.Common.Interface.IFile;
using Couple_Quiz.DTO.Request.Command.User;
using Couple_Quiz.DTO.Response.User;
using Couple_Quiz.Interface.Repositories;
using Couple_Quiz.Models;
using MediatR;

namespace Couple_Quiz.Handler.UserHandler
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserReposne>
    {
        private readonly IAuthRepository authRepository;
        private readonly IFileUpload uploadRepo;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;
        public CreateUserHandler(IAuthRepository authRepository, IFileUpload uploadRepo, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            this.authRepository = authRepository;
            this.uploadRepo = uploadRepo;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }
        public async Task<CreateUserReposne> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var emailExists= await authRepository.FindByEmailAsync(request.Email);
            if (emailExists == null)
            {
                if (request.ProfileImage != null && request.ProfileImage.Length > 0)
                {
                    var imagepath = await uploadRepo.UploadImageAsync(request.ProfileImage);
                    var user = new User()
                    {
                       Name= request.Name,
                       Email= request.Email,
                       Password= request.Password,
                       Age= request.Age,
                       Gender= request.Gender,
                       ProfileImage= imagepath,
                       CreatedAt= System.DateTime.Now, 
                       Active=true,
                    };
                    var newUser = await authRepository.Create(user);
                    var defaultRole = await roleRepository.GetByName();
                    if (defaultRole != null)
                    {
                        var userRole = new UserRole
                        {
                            UserId = newUser.GlobalId,
                            RoleId = defaultRole.GlobalId,
                            Active = true,
                            CreatedAt = System.DateTime.Now
                        };
                        await userRoleRepository.Create(userRole);

                    }
                    return mapper.Map<CreateUserReposne>(user);



                }
                else
                {
                    throw new Exception("Image is Required");
                }
            }
            else
            {
                throw new Exception("User with this Email already Exist");
            }
        }
    }
}
