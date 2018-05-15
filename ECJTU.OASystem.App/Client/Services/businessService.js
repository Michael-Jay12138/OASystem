(function (app) {
    var businessService = function ($http, businessApiUrl) {
        var getAll = function () {
            return $http.get(businessApiUrl + "GetBusinessList");
        };

        var getById = function (id) {
            return $http.get(businessApiUrl + "GetBusinessById/" + id);
        };

        var update = function (Business) {
            return $http.put(businessApiUrl + "UpdateBusiness/" + Business.BusinessId, Business);
        };

        var create = function (Business) {
            console.log(Business);
            return $http.post(businessApiUrl + "CreateBusiness/", Business);
        };

        var destroy = function (Business) {
            return $http.delete(businessApiUrl + "DeleteBusinessById/" + Business.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("businessService", businessService);
}(angular.module("atTheBusiness")))