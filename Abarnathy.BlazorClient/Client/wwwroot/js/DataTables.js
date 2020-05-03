// DataTables initialisation
window.InitDataTable = () =>
    $(document).ready(() =>
        $('#patients-table').DataTable()
    );

window.ReloadDataTable = () => 
    $("#patients-table").DataTable().ajax.Reload();