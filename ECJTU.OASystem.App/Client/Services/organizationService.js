(function (app) {
    var organizationService = function ($http, organizationApiUrl) {
        var getAll = function () {
            return $http.get(organizationApiUrl + "GetOrganizationList");
        };

        var getById = function (id) {
            return $http.get(organizationApiUrl + "GetOrganizationById/" + id);
        };

        var update = function (Organization) {
            return $http.put(organizationApiUrl + "UpdateOrganization/" + Organization.OrganizationId, Organization);
        };

        var create = function (Organization) {
            console.log(Organization);
            return $http.post(organizationApiUrl + "CreateOrganization/", Organization);
        };

        var destroy = function (Organization) {
            return $http.delete(organizationApiUrl + "DeleteOrganizationById/" + Organization.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("organizationService", organizationService);
}(angular.module("atTheOrganization")))