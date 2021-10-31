import { Construct } from "constructs";
import {
  AttributeType,
  BillingMode,
  Table,
} from "aws-cdk-lib/lib/aws-dynamodb";
import { StringParameter } from "aws-cdk-lib/lib/aws-ssm";

export class TestTable extends Construct {
  constructor(
    scope: Construct,
    id: string,
    props: {
      tableName: string;
    }
  ) {
    super(scope, id);

    const table = new Table(this, "Table", {
      tableName: props.tableName,
      partitionKey: {
        name: "__PK",
        type: AttributeType.STRING,
      },
      sortKey: {
        name: "__SK",
        type: AttributeType.STRING,
      },
      billingMode: BillingMode.PAY_PER_REQUEST,
    });

    new StringParameter(this, "TableNameParam", {
      parameterName: "/tsukiy0/extensions/test-table/table-name",
      stringValue: table.tableName,
    });
  }
}
