(function (app) {
    var workitemService = function ($http, workitemApiUrl) {
        var getAll = function () {
            return $http.get(workitemApiUrl + "GetWorkItemList");
        };

        var getById = function (id) {
            return $http.get(workitemApiUrl + "GetWorkItemById/" + id);
        };

        var update = function (WorkItem) {
            return $http.put(workitemApiUrl + "UpdateWorkItem/" + WorkItem.WorkItemId, WorkItem);
        };

        var create = function (WorkItem) {
            console.log(WorkItem);
            return $http.post(workitemApiUrl + "CreateWorkItem/", WorkItem);
        };

        var destroy = function (WorkItem) {
            return $http.delete(workitemApiUrl + "DeleteWorkItemById/" + WorkItem.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("workitemService", workitemService);
}(angular.module("atTheWorkItem")))