import { Construct } from "constructs";
import { AttributeType, BillingMode, Table } from "aws-cdk-lib/lib/aws-dynamodb";
import { IParameter, StringParameter } from "aws-cdk-lib/lib/aws-ssm";

export class TestDynamoTable extends Construct {
  public readonly tableNameParam: IParameter;

  constructor(
    scope: Construct,
    id: string,
  ) {
    super(scope, id);

    const table = new Table(this, "Table", {
      partitionKey: {
        name: "__PK",
        type: AttributeType.STRING
      },
      sortKey: {
        name: "__SK",
        type: AttributeType.STRING
      },
      billingMode: BillingMode.PAY_PER_REQUEST
    });

    const tableNameParam = new StringParameter(this, "TableNameParam", {
      parameterName: "/tsukiy0/extensions/test-dynamo-table/table-name",
      stringValue: table.tableName,
    });

    this.tableNameParam = tableNameParam;
  }
}
