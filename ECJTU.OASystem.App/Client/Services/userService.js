(function (app) {
    var userService = function ($http, userApiUrl) {
        var getAll = function () {
            return $http.get(userApiUrl + "GetUserList");
        };

        var getById = function (id) {
            return $http.get(userApiUrl + "GetUserById/" + id);
        };

        var update = function (User) {
            return $http.put(userApiUrl + "UpdateUser/" + User.UserId, User);
        };

        var create = function (User) {
            console.log(User);
            return $http.post(userApiUrl + "CreateUser/", User);
        };

        var destroy = function (User) {
            return $http.delete(userApiUrl + "DeleteUserById/" + User.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("userService", userService);
}(angular.module("atTheUser")))