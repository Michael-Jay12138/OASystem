(function () {
    var app = angular.module("atTheBusiness", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/business/list", { templateUrl: "../client/views/business/list.html" })
            .otherwise({ redirectTo: "/business/list" });
    }
    app.config(config);
    app.constant("businessApiUrl", "../business/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());