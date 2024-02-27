using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.Service;

namespace LR_12_WEB_NET.Services.UserService;

public interface IUserService: IService
{
    public Task<User?> FindOne(int id);
    public Task<User?> FindOneByEmail(string email);
    public Task<bool> DoesExist(int id);
    public Task<User> AddOne(UserRegisterDto userRegisterDto);
    public Task<User> UpdateOne(int id, UserUpdateDto userUpdateDtoDto);
    public Task<User?> DeleteOne(int id);
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}