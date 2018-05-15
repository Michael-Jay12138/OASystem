(function () {
    var app = angular.module("atTheWorkItem", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/workitem/list", { templateUrl: "../client/views/workitem/list.html" })
            .when("/workitem/check", { templateUrl: "../client/views/workitem/check.html" })
            .otherwise({ redirectTo: "/workitem/list" });
    }
    app.config(config);
    app.constant("workitemApiUrl", "../workitem/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());