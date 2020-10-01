namespace Orleans.Security

module Extensions =
    let (|?) lhs rhs = (if isNull lhs then rhs else lhs)

