(function () {
    var app = angular.module("atTheMaterialTemp", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/materialTemp/list", { templateUrl: "../client/views/materialTemp/list.html" })
            .otherwise({ redirectTo: "/materialTemp/list" });
    }
    app.config(config);
    app.constant("materialTempApiUrl", "../materialTemp/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());