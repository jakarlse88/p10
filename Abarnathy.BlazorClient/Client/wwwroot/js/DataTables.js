// DataTables initialisation
window.InitDataTable = () =>
    $(document).ready(() =>
        $('#patients-table').DataTable()
    );

window.DestroyDataTable = () =>
    $('#patients-table').DataTable().destroy(true);