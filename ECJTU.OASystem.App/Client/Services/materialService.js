(function (app) {
    var materialService = function ($http, materialApiUrl) {
        var getAll = function () {
            return $http.get(materialApiUrl + "GetMaterialList");
        };

        var getById = function (id) {
            return $http.get(materialApiUrl + "GetMaterialById/" + id);
        };

        var update = function (Material) {
            return $http.put(materialApiUrl + "UpdateMaterial/" + Material.MaterialId, Material);
        };

        var create = function (Material) {
            console.log(Material);
            return $http.post(materialApiUrl + "CreateMaterial/", Material);
        };

        var destroy = function (Material) {
            return $http.delete(materialApiUrl + "DeleteMaterialById/" + Material.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("materialService", materialService);
}(angular.module("atTheMaterial")))