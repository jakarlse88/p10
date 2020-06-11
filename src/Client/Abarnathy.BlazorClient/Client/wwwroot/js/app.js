// DataTables initialisation
window.InitDataTable = (tableName) =>
    $(document).ready(() => {
            const selector = `#${tableName}`;
            console.log("Init", selector);
            $(selector).DataTable()
        }
    );

// Patients DataTable cleanup
window.DestroyPatientsTable = () => 
{
    console.log("Destroy patients table");
    $("#patients-table").DataTable().destroy(true);
}

// Notes DataTable cleanup
window.DestroyNoteTable = () =>
    $("#notes-table").DataTable().destroy(true);

window.DestroyAuditLogTable = () =>
    $("#auditlog-table").DataTable().destroy(true);

window.ForceReload = () =>
    window.location.reload(true);

window.NavigateBack = () => 
    window.history.back();