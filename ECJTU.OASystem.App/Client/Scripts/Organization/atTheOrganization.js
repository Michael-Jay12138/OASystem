(function () {
    var app = angular.module("atTheOrganization", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/organization/list", { templateUrl: "../client/views/organization/list.html" })
            .otherwise({ redirectTo: "/organization/list" });
    }
    app.config(config);
    app.constant("organizationApiUrl", "../organization/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());