(function (app) {
    var formTempService = function ($http, formTempApiUrl) {
        var getAll = function () {
            return $http.get(formTempApiUrl + "GetFormTempList");
        };

        var getById = function (id) {
            return $http.get(formTempApiUrl + "GetFormTempById/" + id);
        };

        var update = function (FormTemp) {
            return $http.put(formTempApiUrl + "UpdateFormTemp/" + FormTemp.FormTempId, FormTemp);
        };

        var create = function (FormTemp) {
            console.log(FormTemp);
            return $http.post(formTempApiUrl + "formHtml2htmlFile/", FormTemp);
        };

        var destroy = function (FormTemp) {
            return $http.delete(formTempApiUrl + "DeleteFormTempById/" + FormTemp.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("formTempService", formTempService);
}(angular.module("atTheFormTemp")))