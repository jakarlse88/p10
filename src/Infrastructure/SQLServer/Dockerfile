FROM microsoft/mssql-server-linux:latest

RUN mkdir -p /usr/work

WORKDIR /usr/work

COPY Infrastructure/SQLServer/. /usr/work/

RUN chmod +x /usr/work/migrate.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh