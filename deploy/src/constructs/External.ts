import { Arn, Stack } from "aws-cdk-lib";
import { ITable, Table } from "aws-cdk-lib/lib/aws-dynamodb";
import { IVpc, SubnetType, Vpc } from "aws-cdk-lib/lib/aws-ec2";
import { IGrantable, PolicyStatement } from "aws-cdk-lib/lib/aws-iam";
import { Construct } from "constructs";

export class External extends Construct {
  public readonly vpc: IVpc;
  private readonly table: ITable;

  public constructor(
    scope: Construct,
    id: string,
    private readonly props: { stack: Stack; tableName: string }
  ) {
    super(scope, id);

    const vpc = new Vpc(this, "Vpc", {
      cidr: Vpc.DEFAULT_CIDR_RANGE,
      natGateways: 0,
      subnetConfiguration: [
        {
          name: "public",
          subnetType: SubnetType.PUBLIC,
        },
      ],
    });

    const table = Table.fromTableAttributes(this, "Table", {
      tableName: props.tableName,
    });

    this.vpc = vpc;
    this.table = table;
  }

  public grantTableReadWrite = (grantee: IGrantable): void => {
    this.grantReadParam(grantee, "tsukiy0/extensions/test-table/table-name");
    this.table.grantReadWriteData(grantee);
  };

  public grantReadParam = (grantee: IGrantable, key: string): void => {
    grantee.grantPrincipal.addToPrincipalPolicy(
      PolicyStatement.fromJson({
        Effect: "Allow",
        Action: ["ssm:GetParameter"],
        Resource: [
          Arn.format(
            {
              service: "ssm",
              resource: "parameter",
              resourceName: key,
            },
            this.props.stack
          ),
        ],
      })
    );
  };
}
