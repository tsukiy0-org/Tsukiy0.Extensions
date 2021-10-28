import { Arn, Stack } from "aws-cdk-lib";
import { IVpc, SubnetType, Vpc } from "aws-cdk-lib/lib/aws-ec2";
import { IGrantable, PolicyStatement } from "aws-cdk-lib/lib/aws-iam";
import { Construct } from "constructs";

export class External extends Construct {
  public readonly vpc: IVpc;

  public constructor(
    scope: Construct,
    id: string,
    private readonly props: { stack: Stack }
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

    this.vpc = vpc;
  }

  public grantReadParam = (grantable: IGrantable, key: string): void => {
    grantable.grantPrincipal.addToPrincipalPolicy(
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
