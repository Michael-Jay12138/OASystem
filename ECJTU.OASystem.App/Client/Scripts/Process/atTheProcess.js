(function () {
    var app = angular.module("atTheProcess", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/process/list", { templateUrl: "../client/views/process/list.html" })
            .otherwise({ redirectTo: "/process/list" });
    }
    app.config(config);
    app.constant("processApiUrl", "../process/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());