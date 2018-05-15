(function (app) {
    var activityService = function ($http, activityApiUrl) {
        var getAll = function () {
            return $http.get(activityApiUrl + "GetActivityList");
        };

        var getById = function (id) {
            return $http.get(activityApiUrl + "GetActivityById/" + id);
        };

        var update = function (Activity) {
            return $http.put(activityApiUrl + "UpdateActivity/" + Activity.ActivityId, Activity);
        };

        var create = function (Activity) {
            console.log(Activity);
            return $http.post(activityApiUrl + "CreateActivity/", Activity);
        };

        var destroy = function (Activity) {
            return $http.delete(activityApiUrl + "DeleteActivityById/" + Activity.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("activityService", activityService);
}(angular.module("atTheActivity")))