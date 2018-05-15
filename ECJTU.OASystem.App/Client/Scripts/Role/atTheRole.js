(function () {
    var app = angular.module("atTheRole", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/role/list", { templateUrl: "../client/views/role/list.html" })
            .otherwise({ redirectTo: "/role/list" });
    }
    app.config(config);
    app.constant("roleApiUrl", "../role/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());