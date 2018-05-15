(function (app) {
    var roleService = function ($http, roleApiUrl) {
        var getAll = function () {
            return $http.get(roleApiUrl + "GetRoleList");
        };

        var getById = function (id) {
            return $http.get(roleApiUrl + "GetRoleById/" + id);
        };

        var update = function (Role) {
            return $http.put(roleApiUrl + "UpdateRole/" + Role.RoleId, Role);
        };

        var create = function (Role) {
            return $http.post(roleApiUrl + "CreateRole/", Role);
        };

        var destroy = function (Role) {
            return $http.delete(roleApiUrl + "DeleteRoleById/" + Role.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("roleService", roleService);
}(angular.module("atTheRole")))