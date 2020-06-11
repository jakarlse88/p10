db.createUser({
    user: "historyUser",
    pwd: "Docker__123",
    roles: [
        {
            role: "readWrite",
            db: "HistoryDB"
        }
    ]
});