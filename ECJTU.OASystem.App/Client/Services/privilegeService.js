(function (app) {
    var privilegeService = function ($http, privilegeApiUrl) {
        var getAll = function () {
            return $http.get(privilegeApiUrl + "GetPrivilegeList");
        };

        var getById = function (id) {
            return $http.get(privilegeApiUrl + "GetPrivilegeById/" + id);
        };

        var update = function (Privilege) {
            return $http.put(privilegeApiUrl + "UpdatePrivilege/" + Privilege.PrivilegeId, Privilege);
        };

        var create = function (Privilege) {
            console.log(Privilege);
            return $http.post(privilegeApiUrl + "CreatePrivilege/", Privilege);
        };

        var destroy = function (Privilege) {
            return $http.delete(privilegeApiUrl + "DeletePrivilegeById/" + Privilege.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("privilegeService", privilegeService);
}(angular.module("atThePrivilege")))