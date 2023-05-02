package com.pauloreis.catalog.admin.infrastructure.category;

import com.pauloreis.catalog.admin.domain.category.Category;
import com.pauloreis.catalog.admin.domain.category.CategoryGateway;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.domain.category.CategorySearchQuery;
import com.pauloreis.catalog.admin.domain.pagination.Pagination;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryJpaEntity;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.data.domain.Sort.Direction;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.stereotype.Service;

import java.util.Optional;

import static com.pauloreis.catalog.admin.infrastructure.utils.SpecificationUtils.like;

@Service
public class CategoryMySQLGateway implements CategoryGateway {

  private final CategoryRepository repository;

  public CategoryMySQLGateway(final CategoryRepository repository) {
    this.repository = repository;
  }

  @Override
  public Category create(final Category aCategory) {
    return save(aCategory);
  }

  @Override
  public void deleteById(final CategoryID anId) {
    final String anIdValue = anId.getValue();
    final boolean checkCategoryExists = repository.existsById(anIdValue);

    if (checkCategoryExists) {
      repository.deleteById(anIdValue);
    }
  }

  @Override
  public Pagination<Category> findAll(final CategorySearchQuery aQuery) {
    final var page = PageRequest.of(
        aQuery.page(),
        aQuery.perPage(),
        Sort.by(Direction.fromString(aQuery.direction()), aQuery.sort()));

    final var specifications = Optional.ofNullable(aQuery.terms())
        .filter(str -> !str.isBlank())
        .map(str -> {
          final Specification<CategoryJpaEntity> nameLike = like("name", str);
          final Specification<CategoryJpaEntity> descriptionLike = like("description", str);

          return nameLike.or(descriptionLike);
        })
        .orElse(null);

    final var pageResult = repository.findAll(Specification.where(specifications), page);

    return new Pagination<>(
        pageResult.getNumber(),
        pageResult.getSize(),
        pageResult.getTotalElements(),
        pageResult.map(CategoryJpaEntity::toAggregate).toList());
  }

  @Override
  public Optional<Category> findById(final CategoryID anId) {
    return repository.findById(anId.getValue()).map(CategoryJpaEntity::toAggregate);
  }

  @Override
  public Category update(final Category aCategory) {
    return save(aCategory);
  }

  private Category save(final Category aCategory) {
    return repository.save(CategoryJpaEntity.from(aCategory)).toAggregate();
  }
}
