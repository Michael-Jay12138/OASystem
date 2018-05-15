(function () {
    var app = angular.module("atTheProject", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/project/list", { templateUrl: "../client/views/project/list.html" })
            .when("/project/create", { templateUrl: "../client/views/project/create.html" })
            .otherwise({ redirectTo: "/project/list" });
    }
    app.config(config);
    app.constant("projectApiUrl", "../project/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());