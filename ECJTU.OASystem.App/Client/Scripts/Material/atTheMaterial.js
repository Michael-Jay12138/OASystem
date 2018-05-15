(function () {
    var app = angular.module("atTheMaterial", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/material/list", { templateUrl: "../client/views/material/list.html" })
            .otherwise({ redirectTo: "/material/list" });
    }
    app.config(config);
    app.constant("materialApiUrl", "../material/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());