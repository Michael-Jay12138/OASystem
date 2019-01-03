(function (app) {
    var materialTempService = function ($http, materialTempApiUrl) {
        var getAll = function () {
            return $http.get(materialTempApiUrl + "GetMaterialTempList");
        };

        var getById = function (id) {
            return $http.get(materialTempApiUrl + "GetMaterialTempById/" + id);
        };

        var update = function (MaterialTemp, BuisnessId) {
            return $http.put(materialTempApiUrl + "UpdateMaterialTemp/" + BuisnessId, MaterialTemp);
        };

        var create = function (MaterialTemp,BuisnessId) {
            console.log(MaterialTemp);
            return $http.post(materialTempApiUrl + "CreateMaterialTemp", { businessId: BuisnessId, materialTemp: MaterialTemp });
        };

        var destroy = function (MaterialTemp) {
            return $http.delete(materialTempApiUrl + "DeleteMaterialTempById/" + MaterialTemp.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("materialTempService", materialTempService);
}(angular.module("atTheMaterialTemp")))