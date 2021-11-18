"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.External = void 0;
const aws_cdk_lib_1 = require("aws-cdk-lib");
const aws_ec2_1 = require("aws-cdk-lib/lib/aws-ec2");
const aws_iam_1 = require("aws-cdk-lib/lib/aws-iam");
const constructs_1 = require("constructs");
class External extends constructs_1.Construct {
    constructor(scope, id, props) {
        super(scope, id);
        this.props = props;
        this.grantReadParam = (grantable, key) => {
            grantable.grantPrincipal.addToPrincipalPolicy(aws_iam_1.PolicyStatement.fromJson({
                Effect: "Allow",
                Action: ["ssm:GetParameter"],
                Resource: [
                    aws_cdk_lib_1.Arn.format({
                        service: "ssm",
                        resource: "parameter",
                        resourceName: key,
                    }, this.props.stack),
                ],
            }));
        };
        const vpc = new aws_ec2_1.Vpc(this, "Vpc", {
            cidr: aws_ec2_1.Vpc.DEFAULT_CIDR_RANGE,
            natGateways: 0,
            subnetConfiguration: [
                {
                    name: "public",
                    subnetType: aws_ec2_1.SubnetType.PUBLIC,
                },
            ],
        });
        this.vpc = vpc;
    }
}
exports.External = External;
//# sourceMappingURL=External.js.map