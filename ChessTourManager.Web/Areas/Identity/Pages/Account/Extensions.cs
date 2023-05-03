using ChessTourManager.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChessTourManager.WEB.Areas.Identity.Pages.Account;

public static class Extensions
{
    public static async Task SetUserFirstNameAsync(this IUserStore<User> userStore,
                                                   User                  user,
                                                   string                firstName,
                                                   CancellationToken     cancellationToken = default)
    {
        user.UserFirstName = firstName;
//        await userStore.UpdateAsync(user, cancellationToken);
    }

    public static async Task SetUserLastNameAsync(this IUserStore<User> userStore,
                                                  User                  user,
                                                  string                lastName,
                                                  CancellationToken     cancellationToken = default)
    {
        user.UserLastName = lastName;
//        await userStore.UpdateAsync(user, cancellationToken);
    }

    public static async Task SetUserPatronymicAsync(this IUserStore<User> userStore,
                                                    User                  user,
                                                    string                patronymic,
                                                    CancellationToken     cancellationToken = default)
    {
        user.UserPatronymic = patronymic;
//        await userStore.UpdateAsync(user, cancellationToken);
    }
}
