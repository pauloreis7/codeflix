package com.pauloreis.catalog.admin.application.category.retrieve.list;

import com.pauloreis.catalog.admin.application.UseCase;
import com.pauloreis.catalog.admin.domain.category.CategorySearchQuery;
import com.pauloreis.catalog.admin.domain.pagination.Pagination;

public abstract class ListCategoriesUseCase
    extends UseCase<CategorySearchQuery, Pagination<CategoryListOutput>> {

}
